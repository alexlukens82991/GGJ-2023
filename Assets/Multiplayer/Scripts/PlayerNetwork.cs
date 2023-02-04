using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private Transform m_SpawnedObjectPrefab;

    private NetworkVariable<int> m_RandomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        m_RandomNumber.OnValueChanged += (int prev, int newVal) => 
        {
            Debug.Log(OwnerClientId + " | " + m_RandomNumber.Value);
        };
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Transform spawnedObj = Instantiate(m_SpawnedObjectPrefab);
            spawnedObj.GetComponent<NetworkObject>().Spawn(true);
        }
    }

    private void FixedUpdate()
    {

        if (!IsOwner)
        {
            return;
        }


        Vector3 moveDir = new();

        if (Input.GetKey(KeyCode.W)) moveDir.z = 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1f;

        float moveSpeed = 3f;

        transform.position += moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    [ServerRpc] // code that only runs on the server. called from client
    private void TestServerRpc()
    {
        print("TestServerRpc " + OwnerClientId);
    }

    [ClientRpc]
    private void TestClientRpc()
    {
        print("testClientRpc");
    }
}
