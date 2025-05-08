using UnityEngine;

[CreateAssetMenu(fileName = "NewKomaSO", menuName = "ScriptableObjects/Koma/KomaSO")]
public class KomaSO : ScriptableObject
{
    [field: SerializeField] public KomaMovementData MovementData {  get; private set; }
    [field: SerializeField] public KomaStatusData Status { get; private set; }
    [field: SerializeField] public KomaSkillsData SkillData { get; private set; }
}
