using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class CutsceneDirector : MonoBehaviour
{
    [System.Serializable]
    public struct Eyes
    {
        public GameObject leftEyeVFX;
        public GameObject rightEyeVFX;
    }

    public GameObject ninjaA;
    public GameObject ninjaB;
    public Transform ninjaAHead;
    public Transform ninjaBHead;
    public Eyes ninjaAEyes;
    public Eyes ninjaBEyes;
    public GameObject ninjaALandVFX;
    public GameObject ninjaBLandVFX;
    public CinemachineVirtualCamera cameraToFix;
    public FadeCamera cameraFader;
    public Animation finalCamZoomOutAnim;

    private float ninjaAHeadRotationSpeed = 0.3f;
    private bool ninjaALookAtNinjaB = false;
    private float ninjaAHeadRotationProgress = 0.0f;
    private Vector3 relativePositionAtoB;
    private Quaternion targetRotationAtoB;

    private float ninjaBHeadRotationSpeed = 1.0f;
    private bool ninjaBLookAtNinjaA = false;
    private float ninjaBHeadRotationProgress = 0.0f;
    private Vector3 relativePositionBtoA;
    private Quaternion targetRotationBtoA;

    private void Start()
    {
        NinjaALookAtNinjaB();
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        if (ninjaALookAtNinjaB)
        {
            if (ninjaAHeadRotationProgress > 2)
            {
                ninjaAHead.LookAt(ninjaBHead);
            }
            else
            {
                ninjaAHead.rotation = Quaternion.Slerp(ninjaAHead.rotation, targetRotationAtoB, ninjaAHeadRotationProgress);
                ninjaAHeadRotationProgress += Time.deltaTime * ninjaAHeadRotationSpeed;
            }
        }

        if (ninjaBLookAtNinjaA)
        {
            if (ninjaBHeadRotationProgress > 1)
            {
                ninjaBHead.LookAt(ninjaAHead);
            }
            else
            {
                ninjaBHead.rotation = Quaternion.Slerp(ninjaBHead.rotation, targetRotationBtoA, ninjaBHeadRotationProgress);
                ninjaBHeadRotationProgress += Time.deltaTime * ninjaBHeadRotationSpeed;
            }
        }
    }

    public void NinjaALookAtNinjaB()
    {
        relativePositionAtoB = ninjaBHead.position - ninjaAHead.position;
        targetRotationAtoB = Quaternion.LookRotation(relativePositionAtoB);
        ninjaALookAtNinjaB = true;
    }

    public void NinjaBLookAtNinjaA()
    {
        relativePositionBtoA = ninjaAHead.position - ninjaBHead.position;
        targetRotationBtoA = Quaternion.LookRotation(relativePositionBtoA);
        ninjaBLookAtNinjaA = true;
    }

    public void NinjaAGlowingEyes()
    {
        ninjaAEyes.leftEyeVFX.gameObject.SetActive(true);
        ninjaAEyes.rightEyeVFX.gameObject.SetActive(true);
    }

    public void NinjaBGlowingEyes()
    {
        ninjaBEyes.leftEyeVFX.gameObject.SetActive(true);
        ninjaBEyes.rightEyeVFX.gameObject.SetActive(true);
    }

    public void NinjaALand()
    {
        ninjaALandVFX.gameObject.SetActive(true);
    }

    public void NinjaBLand()
    {
        ninjaBLandVFX.gameObject.SetActive(true);
    }

    public void FixCameraDipBug()
    {
        var transposer = cameraToFix.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (transposer != null)
        {
            transposer.m_LookaheadIgnoreY = true;
            transposer.m_YDamping = 30;
        }
    }

    public void EndOfCutsceneCameraFade()
    {
        cameraFader.FadeOut();
        finalCamZoomOutAnim.Play();
    }

    private Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;
            Transform result = FindChildRecursive(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
}
