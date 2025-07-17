using UnityEngine;

// Item이라는 Data에셋을 만들 수 있게 설정
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    // 아이템 종류
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    // 기본 정보
    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    // 레벨 데이터
    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    // 무기
    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}
