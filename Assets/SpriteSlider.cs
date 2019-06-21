using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteSlider : MonoBehaviour
{
	public enum SliderType
	{
		CenterFill,
		HorizontalCenterFill,
		VerticalCenterFill,
		LeftToRightFill,
		RightToLeftFill,
		UpToDownFill,
		DownToUpFill
	};

	[SerializeField] private SliderType type;
	
	[Range(0f,1f)]
	public float fillValue = 1f;

	public float positionOffsetFactor = 0f;

	public Vector3 rootPosition;
	public float maxScaleX;
	public float maxScaleY;

    void Start()
    {
    }
	
    void Update()
    {
		Vector2 scale = transform.localScale;
		Vector3 pos = transform.localPosition;

		if (type == SliderType.CenterFill)
		{
			scale.x = maxScaleX * fillValue;
			scale.y = maxScaleY * fillValue;
		}
		else if (type == SliderType.HorizontalCenterFill)
		{
			scale.x = maxScaleX * fillValue;
		}
		else if (type == SliderType.VerticalCenterFill)
		{
			scale.y = maxScaleY * fillValue;
		}
		else if (type == SliderType.LeftToRightFill)
		{
			scale.x = maxScaleX * fillValue;
			pos.x = rootPosition.x - ((1f - fillValue) * positionOffsetFactor);
		}
		else if (type == SliderType.RightToLeftFill)
		{
			scale.x = maxScaleX * fillValue;
			pos.x = rootPosition.x + ((1f - fillValue) * positionOffsetFactor);
		}
		else if (type == SliderType.UpToDownFill)
		{
			scale.y = maxScaleY * fillValue;
			pos.y = rootPosition.y + ((1f - fillValue) * positionOffsetFactor);
		}
		else if (type == SliderType.DownToUpFill)
		{
			scale.y = maxScaleY * fillValue;
			pos.y = rootPosition.y + ((1f - fillValue) * positionOffsetFactor);
		}

		transform.localScale = scale;
		transform.localPosition = pos;
	}
}
