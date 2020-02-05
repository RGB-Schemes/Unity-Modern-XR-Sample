using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRPlayer : MonoBehaviour
{
    public GameObject Head;
    public Rigidbody LeftHand, RightHand;

    private List<XRNodeState> mNodeStates = new List<XRNodeState>();
    private Vector3 mHeadPos, mLeftHandPos, mRightHandPos;
    private Quaternion mHeadRot, mLeftHandRot, mRightHandRot;

    private void Start()
    {
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
        for (int i = 0; i < subsystems.Count; i++)
        {
            subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
        }
    }

    private void Update()
    {
        InputTracking.GetNodeStates(mNodeStates);

        foreach (XRNodeState nodeState in mNodeStates)
        {
            switch (nodeState.nodeType)
            {
                case XRNode.Head:
                    nodeState.TryGetPosition(out mHeadPos);
                    nodeState.TryGetRotation(out mHeadRot);
                    break;
            }
        }
        Head.transform.position = mHeadPos;
        Head.transform.rotation = mHeadRot.normalized;
    }

    // FixedUpdate stays in sync with the physics engine.
    private void FixedUpdate()
    {
        InputTracking.GetNodeStates(mNodeStates);

        foreach (XRNodeState nodeState in mNodeStates)
        {
            switch (nodeState.nodeType)
            {
                case XRNode.LeftHand:
                    nodeState.TryGetPosition(out mLeftHandPos);
                    nodeState.TryGetRotation(out mLeftHandRot);
                    break;
                case XRNode.RightHand:
                    nodeState.TryGetPosition(out mRightHandPos);
                    nodeState.TryGetRotation(out mRightHandRot);
                    break;
            }
        }

        LeftHand.MovePosition(mLeftHandPos);
        LeftHand.MoveRotation(mLeftHandRot.normalized);
        RightHand.MovePosition(mRightHandPos);
        RightHand.MoveRotation(mRightHandRot.normalized);
    }
}
