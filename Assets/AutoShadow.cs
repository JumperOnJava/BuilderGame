using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
[ExecuteInEditMode]
[RequireComponent(typeof(ShadowCaster2D))]
[RequireComponent(typeof(SpriteShapeController))]
public class AutoShadow : MonoBehaviour
{
	//���� ���� ����������� ���������� ����� �� ��� SpriteShape
	private ShadowCaster2D _shadowCaster;
	private SpriteShapeController _spriteShapeController;
	private Spline _spline;
	void Start()
    {
		_shadowCaster = GetComponent<ShadowCaster2D>();
		_spriteShapeController = GetComponent<SpriteShapeController>();
		_spline = _spriteShapeController.spline;

		var splineObject = _spline;
		var shadowCasterType = typeof(ShadowCaster2D);
		//�������� ������ � ���������� � ����� ��������� ����, �������� ��������� ������
		//�������� �� ����� SpriteShape

		List<SplineControlPoint> privatePoints =
			(List<SplineControlPoint>)typeof(Spline)
			.GetField("m_ControlPoints", BindingFlags.NonPublic | BindingFlags.Instance)
			.GetValue(splineObject);

		var points = new List<Vector3>();
		//������������ �� ����� SpriteShape � ����� �������
		foreach (SplineControlPoint point in privatePoints)
		{
			points.Add(point.position);
		}
		//������������ �� ����� �� ����� ������� ���� �� ��������
		var shapeField = shadowCasterType.GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
		shapeField.SetValue(_shadowCaster, points.ToArray());

		//��������� ����� ��
		var meshField = shadowCasterType.GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
		meshField.SetValue(_shadowCaster, new Mesh());
		typeof(ShadowCaster2D)
			.Assembly
			.GetType("UnityEngine.Rendering.Universal.ShadowUtility")
			.GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static)
			.Invoke(_shadowCaster, new object[] { meshField.GetValue(_shadowCaster), shapeField.GetValue(_shadowCaster) });
		Debug.Log("bruh");
		DestroyImmediate(this);
	}
}
