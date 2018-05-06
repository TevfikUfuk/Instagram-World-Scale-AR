using UnityEngine;

namespace Scripts.Utilities
{
	public class Billboard : MonoBehaviour
	{
		[SerializeField]
		Camera _camera;

		[SerializeField]
		bool _lockX;

		Transform _transform;

		Quaternion _originalRotation;

		public Camera BillboardCamera
		{
			set
			{
				_camera = value;
			}
		}

		void Awake()
		{
			_transform = transform;
			_originalRotation = _transform.rotation;
			if (_camera == null)
			{
				_camera = Camera.main;
			}
		}

		void Update()
		{
			if (_camera == null)
			{
				return;
			}

			var lookPos = _transform.position - _camera.transform.position;

			if (_lockX)
			{
				lookPos.y = 0f;
			}

			var rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = _originalRotation * (rotation);
		}
	}
}
