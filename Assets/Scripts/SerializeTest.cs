using UnityEngine;
using System.Collections;

public class SerializeTest : MonoBehaviour {

    private void OnSerializeNetworkStream(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
            stream.Serialize(ref pos);
        }
        else
        {
            Vector3 pos = Vector3.zero;
            stream.Serialize(ref pos);

            transform.position = pos;
        }

    }

}
