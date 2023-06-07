using UnityEngine;

namespace GlobalConstants
{
    public static class Constants
    {
        [Header("SITUATIONAL")]

        [Header("PARAMETERS ANIMATIONS")]
        public const string ATTACK = "Attack";
        public const string WALK = "Walk";
        public const string GETHIT = "GetHit";
        public const string DODGE = "Dodge";
        public const string DIE = "Die";

        [Header("LAYERS INDEX")]
        public const int ENEMY = 7;
        public const int PLAYER = 6;
        public const int OBJECT_DESTRUCTABLE = 8;
    }
}
//[CreateAssetMenu(fileName = "NewConstantAnim", menuName = "ScriptableObjects/ConstantAnimator", order = 1)]
