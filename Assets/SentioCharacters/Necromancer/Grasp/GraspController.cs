using UnityEngine;
using System.Collections;

public class GraspController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _grasp = null;

    [SerializeField]
    private Light _graspLight = null;

    [SerializeField]
    private Projector _graspGround = null;

    [SerializeField]
    private float _initialVelocity = 2;

    private float _initialAspectRatio = 1;
    private float _offsetRandom = 0;

    private Vector3 _startPosition = Vector3.zero;

    private float _startTime = 0;
    private float _duration = 0;

    public void Start()
    {
        _initialAspectRatio = _graspGround.aspectRatio;
        _offsetRandom = _initialAspectRatio * Random.value;
    }

    public void ThrowGrasp(Vector3 aStart, Vector3 aDirection, float aDuration)
    {
        gameObject.transform.rotation = Quaternion.LookRotation(aDirection);
        gameObject.transform.position = aStart;

        // We copy this because I don't seem to have access to the proper property blocks
        // so we set our properties directly on the copied material
        _graspGround.material = new Material(_graspGround.material);

        _startPosition = aStart;

        _startTime = Time.time;
        _duration = aDuration;

        _grasp.Simulate(0);
        _grasp.Play();

        GetComponent<AudioSource>().Play();
    }

    public void LateUpdate()
    {
        // A lot of magic happening here, some comments to help it out
        if (Time.time <= (_startTime + _duration))
        {
            // Move by our initial velocity, right now this is constant
            gameObject.transform.position += gameObject.transform.forward * _initialVelocity * Time.deltaTime;

            // Grab our distance for a few calculations
            float tDistance = (gameObject.transform.position - _startPosition).magnitude;

            // We scale our projector out and adjust its position to keep it from overlapping our caster
            // the next two lines are scaling it and keeping it in the center
            _graspGround.gameObject.transform.localPosition = new Vector3(_graspGround.gameObject.transform.localPosition.x,
                                                                          _graspGround.gameObject.transform.localPosition.y,
                                                                          1 + -tDistance / 2);
            _graspGround.aspectRatio = tDistance / (_graspGround.orthographicSize * 2);

            // Our texture has a certain ratio, this keeps everything in scale even though we are messing with the aspect ratio above
            _graspGround.material.SetFloat("_AspectMultiplier", _graspGround.aspectRatio / _initialAspectRatio);
            // Our pixel multiplier, this helps us convert our uvs to world space
            _graspGround.material.SetFloat("_PixelMultiplier", _graspGround.aspectRatio * _graspGround.orthographicSize * 2);
            // Our distance traveled for projector purposes
            _graspGround.material.SetFloat("_Distance", tDistance);
            // Our distance traveled for animation purposes
            _graspGround.material.SetFloat("_AnimationDistance", tDistance);
            // This is our random offset
            _graspGround.material.SetFloat("_Offset", _offsetRandom);
        }
        else
        {
            Vector3 tAnimationPosition = gameObject.transform.position + gameObject.transform.forward * _initialVelocity * (Time.time - (_startTime + _duration));
            float tDistance = (gameObject.transform.position - _startPosition).magnitude;
            float tAnimationDistance = (tAnimationPosition - _startPosition).magnitude;

            _grasp.Stop();

            GetComponent<AudioSource>().Stop();

            // Get our color and fade it out over a second (we may want to get the dying particle length here)
            Color tColor = _graspGround.material.GetColor("_Color");
            tColor.a = 1 - Mathf.Clamp01(Time.time - (_startTime + _duration));
            _graspGround.material.SetColor("_Color", tColor); 

            // Have to set these here to make sure they get the correct final value
            _graspGround.material.SetFloat("_PixelMultiplier", _graspGround.aspectRatio * _graspGround.orthographicSize * 2);
            _graspGround.material.SetFloat("_Distance", tDistance);
            _graspGround.material.SetFloat("_AnimationDistance", tAnimationDistance);
            _graspGround.material.SetFloat("_Offset", _offsetRandom);

            // And fade our light the same as the color
            _graspLight.intensity = 1 - Mathf.Clamp01(Time.time - _duration);
        }

        if (!_grasp.IsAlive())
            Object.Destroy(gameObject);
	}
}
