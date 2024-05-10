using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private CinemachineVirtualCamera virutalCamera;

    private CinemachineImpulseSource _impulseSource;

    public VolumeProfile VolumeProfile { get => volumeProfile; }
    public CinemachineImpulseSource ImpulseSource { get => _impulseSource; }

    public float CameraZoom { get => virutalCamera.m_Lens.FieldOfView; set => virutalCamera.m_Lens.FieldOfView = value; }

    private void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera()
    {
        _impulseSource.GenerateImpulse();
    }
    public void ShakeCamera(Vector3 force)
    {
        _impulseSource.GenerateImpulse(force);
    }
}
