using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PluginExtension
{
	public static void SetX(this Transform transform, float x)
	{
		Vector3 position = new Vector3(x, transform.position.y, transform.position.z);
		transform.position = position;
	}

	public static void SetY(this Transform transform, float y)
	{
		Vector3 position = new Vector3(transform.position.x, y, transform.position.z);
		transform.position = position;
	}

	public static void SetZ(this Transform transform, float z)
	{
		Vector3 position = new Vector3(transform.position.x, transform.position.y, z);
		transform.position = position;
	}

	public static void SetPosition2D(this Transform transform, Vector3 target)
	{
		Vector3 position = new Vector3(target.x, target.y, transform.position.z);
		transform.position = position;
	}

	public static void SetLocalX(this Transform transform, float x)
	{
		Vector3 localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
		transform.localPosition = localPosition;
	}

	public static void SetLocalY(this Transform transform, float y)
	{
		Vector3 localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
		transform.localPosition = localPosition;
	}

	public static void SetLocalZ(this Transform transform, float z)
	{
		Vector3 localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
		transform.localPosition = localPosition;
	}

	public static void MoveLocalX(this Transform transform, float deltaX)
	{
		Vector3 localPosition = new Vector3(transform.localPosition.x + deltaX, transform.localPosition.y, transform.localPosition.z);
		transform.localPosition = localPosition;
	}

	public static void MoveLocalY(this Transform transform, float deltaY)
	{
		Vector3 localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + deltaY, transform.localPosition.z);
		transform.localPosition = localPosition;
	}

	public static void MoveLocalZ(this Transform transform, float deltaZ)
	{
		Vector3 localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + deltaZ);
		transform.localPosition = localPosition;
	}

	public static void MoveLocalXYZ(this Transform transform, float deltaX, float deltaY, float deltaZ)
	{
		Vector3 localPosition = new Vector3(transform.localPosition.x + deltaX, transform.localPosition.y + deltaY, transform.localPosition.z + deltaZ);
		transform.localPosition = localPosition;
	}

	public static void SetLocalScaleX(this Transform transform, float x)
	{
		Vector3 localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
		transform.localScale = localScale;
	}

	public static void SetLocalScaleY(this Transform transform, float y)
	{
		Vector3 localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
		transform.localScale = localScale;
	}

	public static void SetLocalScaleZ(this Transform transform, float z)
	{
		Vector3 localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
		transform.localScale = localScale;
	}

	public static void Times(this int count, Action action)
	{
		for (int i = 0; i < count; i++)
		{
			action();
		}
	}

	public static void SetColorAlpha(this MaskableGraphic graphic, float a)
	{
		Color color = graphic.color;
		color.a = a;
		graphic.color = color;
	}

	public static void LookAt2D(this Transform transform, Vector3 target, float angle = 0f)
	{
		Vector3 vector = target - transform.position;
		angle += Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
	}

	public static void LookAt2D(this Transform transform, Transform target, float angle = 0f)
	{
		transform.LookAt2D(target.position, angle);
	}

	public static float Delta(this float number, float delta)
	{
		return number + UnityEngine.Random.Range(0f - delta, delta);
	}

	public static float DeltaPercent(this float number, float percent)
	{
		return number.Delta(number * percent);
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		System.Random random = new System.Random();
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = random.Next(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}
}
