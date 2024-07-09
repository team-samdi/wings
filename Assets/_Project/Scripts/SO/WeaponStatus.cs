using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Status", menuName = ("Project Wing/Weapon/Status"), order=int.MinValue)]
public class WeaponStatus : ScriptableObject {

	
	[Range(0f, 1f)] public float aimingTime;
	[Space(5f)]
	[Range(0.1f, 1)] public float adsSwayDisplaceAmount;
	[Range(0.1f, 1)] public float adsSwayTiltAmount;
	[Range(0.1f, 1)] public float adsShakeAmount;
	[Space(15f)]
	[Range(0.1f, 1)] public float weaponSpeed;
	[Range(0.1f, 1)] public float whileAdsSpeed;
}
