
public interface Hitbox
{
    Entity parent { get; }
    HitboxType hitboxType { get; }
}
public enum HitboxType
{
    Damaged,
    Damager
}