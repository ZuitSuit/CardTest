using UnityEngine;

public class CardLayoutGroup : MonoBehaviour
{

	[Header("Settings")]
	public float maxAngle;
	public float spacing;

	public AnimationCurve yOffset;
	public float yOffsetMultiplier = 1f;

	public float yLinearOffset = -20f;

	void Update()
	{
		int childCount = transform.childCount;

		float leftXOffset = Mathf.Floor(childCount / 2) * spacing;

		for (int i = 0; i < transform.childCount; i++)
		{
			float perc = ((float)i + 0.5f) / (float)childCount;

			float currentXOffset = Mathf.Lerp(-leftXOffset, leftXOffset, perc);
			float currentRotation = Mathf.Lerp(-maxAngle, maxAngle, perc);

			float currentYOffset = yOffset.Evaluate(perc) * yOffsetMultiplier + yLinearOffset;

			Transform child = transform.GetChild(i);

			Vector3 pos = transform.position + Vector3.left * currentXOffset + Vector3.up * currentYOffset;
			child.position = Vector3.Lerp(child.position, pos, 10f * Time.deltaTime);

			Quaternion targetRot = Quaternion.Euler(0f, 0f, currentRotation);
			child.rotation = Quaternion.Lerp(child.rotation, targetRot, 5f * Time.deltaTime);
		}
	}
}