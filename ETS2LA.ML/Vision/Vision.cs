using Hexa.NET.OpenGL;

using System.Numerics;
using System.Diagnostics;

using ETS2LA.State;
using ETS2LA.Shared;
using ETS2LA.Logging;
using ETS2LA.Game.Telemetry;
using ETS2LA.Game.SDK;

using TruckLib.ScsMap;
using TruckLib;

namespace ETS2LA.ML.Vision;

public class VisionHandler
{
    private static readonly Lazy<VisionHandler> _instance = new(() => new VisionHandler());
    public static VisionHandler Current => _instance.Value;

    public List<VirtualCamera> Cameras { get; private set; } = new();
    public GL? gl;

    private int viewDistance = 300;
    private float nodeUpdateInterval = 5f; // seconds
    private readonly object geometryLock = new();
    private IReadOnlyList<Node> nearbyNodes = new List<Node>();
    private List<Vector3> roadGeometryBuffer = new();
    private Vector3[]? roadVertexScratch;

    private SolidColor? solidColor;
    private RoadMesh? roadMesh;
    private VehicleMesh? vehicleMesh;

    private bool shutdown = false;
    private Thread? updateThread;

    public bool Initialized => gl != null;

    public VisionHandler()
    {
    }

    public void Initialize(GL gl)
    {
        this.gl = gl;
        gl.DepthMask(true);
        
        AddCamera("Front", 480, 480, 
            rotation: Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 10f), 
            offset: new Vector3(0f, -1f, 3f));
        AddCamera("Top", 240, 480, 
            fieldOfView: 14f,
            rotation: Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 2f), 
            offset: new Vector3(0f, -900f, 0f));

        solidColor = new SolidColor(gl);
        roadMesh = new RoadMesh(gl);
        vehicleMesh = new VehicleMesh(gl);

        updateThread = new Thread(UpdateLoop)
        {
            IsBackground = true,
            Name = "VisionHandler-Update"
        };
        updateThread.Start();
    }

    private void UpdateLoop()
    {
        while (!shutdown)
        {
            UpdateNearbyNodes();
            Thread.Sleep((int)(nodeUpdateInterval * 1000));
        }
    }

    private void UpdateNearbyNodes()
    {
        var cameraData = CameraProvider.Current.GetCurrentData();
        if (cameraData == null) return;
        
        Vector3 center = cameraData.truckPosition;
        double minX = center.X - viewDistance;
        double maxX = center.X + viewDistance;
        double minZ = center.Z - viewDistance;
        double maxZ = center.Z + viewDistance;
        var nodes = ApplicationState.Current.RunningGame?.GetMapData()?.Nodes.Within(minX, minZ, maxX, maxZ) ?? new List<Node>();
        var geometry = VisionRoadUtils.BuildRoadGeometry(nodes.ToArray());

        lock (geometryLock)
        {
            nearbyNodes = nodes;
            roadGeometryBuffer = geometry;
        }
    }

    public void Render()
    {
        var cameraData = CameraProvider.Current.GetCurrentData();
        if (gl == null) return;
        if (cameraData == null) return;

        Vector3 center = cameraData.truckPosition;
        Quaternion truckRot = cameraData.truckRotation;
        var euler = truckRot.ToEuler();
        euler.Y = -euler.Y - (float)Math.PI;
        truckRot = Quaternion.CreateFromYawPitchRoll(euler.Y, euler.X, euler.Z);

        List<Vector3> roadSnapshot;
        lock (geometryLock)
        {
            roadSnapshot = roadGeometryBuffer;
        }

        int roadCount = roadSnapshot.Count;
        if (roadVertexScratch == null || roadVertexScratch.Length != roadCount)
            roadVertexScratch = new Vector3[roadCount];

        for (int i = 0; i < roadCount; i++)
            roadVertexScratch[i] = roadSnapshot[i] - center;

        roadMesh?.UpdateVertices(roadVertexScratch);

        var vehicleVertices = VisionVehicleUtils.BuildVehicleGeometry(
                                TrafficProvider.Current.GetCurrentTrafficData(),
                                ParkedVehiclesProvider.Current.GetCurrentParkedVehicleData());
        for(int i = 0; i < vehicleVertices.Count; i++)
            vehicleVertices[i] -= center;
        vehicleMesh?.UpdateVertices(vehicleVertices);

        solidColor?.Use();
        foreach (var camera in Cameras)
        {
            camera.BeginRender();
            solidColor?.SetViewProjection(camera.GetViewProjectionMatrix(Vector3.Zero, truckRot));

            solidColor?.SetColor(new Vector4(0.0f, 1.0f, 0.0f, 1.0f));
            roadMesh?.Draw();

            solidColor?.SetColor(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
            vehicleMesh?.Draw();

            camera.EndRender();
        }
        solidColor?.End();
    }

    public void AddCamera(string name, int width, int height, Quaternion? rotation = null, Vector3? offset = null, float fieldOfView = 90f)
    {
        if (gl == null)
        {
            Logger.Error("VisionHandler: Cannot add camera, GL context is not initialized.");
            return;
        }

        Cameras.Add(new VirtualCamera(name, width, height, gl, rotation ?? Quaternion.Identity, offset ?? Vector3.Zero, fieldOfView));
    }

    public void Shutdown()
    {
        shutdown = true;
        updateThread?.Join(TimeSpan.FromSeconds(2));
    }
}
