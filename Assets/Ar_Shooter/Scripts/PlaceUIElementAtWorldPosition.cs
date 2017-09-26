using UnityEngine;

/// <summary>
/// Place an UI element to a world position
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class PlaceUIElementAtWorldPosition : MonoBehaviour
{
	private RectTransform rectTransform;
	private Vector2 uiOffset;

	/// <summary>
	/// Initiate
	/// </summary>
	void Start ()
	{
		// Get the rect transform
		this.rectTransform = GetComponent<RectTransform>();

		// Calculate the screen offset
		this.uiOffset = new Vector2((float)rectTransform.sizeDelta.x / 2f, (float)rectTransform.sizeDelta.y / 2f);
	}

	/// <summary>
	/// Move the UI element to the world position
	/// </summary>
	/// <param name="objectTransformPosition"></param>
	public void MoveToClickPoint(Vector3 objectTransformPosition)
	{
		// Get the position on the canvas
		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectTransformPosition);
		Vector2 proportionalPosition = new Vector2(ViewportPosition.x * rectTransform.sizeDelta.x, ViewportPosition.y * rectTransform.sizeDelta.y);

		// Set the position and remove the screen offset
		this.rectTransform.localPosition = proportionalPosition - uiOffset;
	}
}