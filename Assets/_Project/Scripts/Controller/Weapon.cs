using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour {

	[SerializeField] private WeaponPlaceSetting placeSetting;
	[SerializeField] private WeaponHandAttachSetting handSetting;
	[SerializeField] private WeaponStatus status;


	public WeaponPlaceSetting PlaceSetting {
		get => placeSetting;
	}

	public WeaponHandAttachSetting HandSetting {
		get => handSetting;
	}

	public WeaponStatus Status {
		get => status;
	}
}