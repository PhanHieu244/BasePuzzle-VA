using System;
using Core.Data;
using UnityEngine;
using View.Items;

namespace View.Game
{
	/// <summary>
	/// Displays a cursor icon hint to show where the user should swipe in the game board.
	/// </summary>
	public class CursorSwipe : MonoBehaviour
	{
		// TODO: make configurable
		private const float Time = 1.8f;
		private const float Delay = 0f;
		private const float MoveDistance = 1.2f;

		private SpriteRenderer _spriteRenderer;
		private Color _initColor;
		private Vector3 _initPos;
		private Vector3 _hotSpot;
			
		private int _colorTween;
		private int _moveTween;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			
			_hotSpot = new Vector2 (_spriteRenderer.bounds.size.x, _spriteRenderer.bounds.size.y) / 2f;

			_initPos = transform.localPosition + Vector3.up * _hotSpot.y / 2f;
			_initColor = _spriteRenderer.color;
			_spriteRenderer.color = Colorizer.Alpha(_spriteRenderer.color, 0f);
		}

		public void Show(Vector3 pos, Direction dir)
		{
			Hide(() => {
				transform.localPosition = _initPos + pos;
				var move = transform.localPosition + dir.Vector() * MoveDistance;

				_moveTween = LeanTween.moveLocal(gameObject, move, Time)
					.setDelay(Delay)
					.setEase(LeanTweenType.easeInOutSine)
					.setLoopCount(-1)
					.setOnCompleteOnStart(true)
					.setOnCompleteOnRepeat(true)
					.setOnComplete(() => {
						LeanTween.cancel(_colorTween);
						_colorTween = LeanTween.value(0f, _initColor.a, Time / 2f)
							.setDelay(Delay)
							.setEase(LeanTweenType.easeInOutSine)
							.setLoopPingPong(1)
							.setOnUpdate(a => {
								// Nên kiểm tra null ở đây nếu component có thể bị hủy riêng
								if (_spriteRenderer != null) 
									_spriteRenderer.color = Colorizer.Alpha(_spriteRenderer.color, a);
							})
							.id;
					})
					.id;	
			});
		}
		
		private void OnDestroy()
		{
			// Hủy tất cả các tween được tạo từ chính GameObject này
			LeanTween.cancel(gameObject); 
    
			// Đảm bảo không gọi TouchKit vì script này không liên quan đến TouchKit
			// Nếu bạn muốn hủy riêng theo ID cũng được, nhưng hủy theo GameObject là an toàn nhất.
			// LeanTween.cancel(_moveTween); 
			// LeanTween.cancel(_colorTween);
		}

		public void Hide(Action onComplete = null)
		{
			LeanTween.cancel(_moveTween);
			LeanTween.cancel(_colorTween);
			
			_colorTween = LeanTween.value(_spriteRenderer.color.a, 0f, Time / 8f)
				.setEase(LeanTweenType.easeInOutSine)
				.setOnUpdate(a => _spriteRenderer.color = Colorizer.Alpha(_spriteRenderer.color, a))
				.setOnComplete(onComplete)
				.id;
		}
	}
}
