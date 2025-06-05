using Cinemachine;
using UnityEngine;
using Zenject;

namespace PocketZoneTest
{
    public class CameraControll : MonoBehaviour
    {

        [Inject]
        public void Construct(Player player)
        {
            GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        }
    }
}
