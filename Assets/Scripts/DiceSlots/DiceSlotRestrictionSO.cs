using UnityEngine;

/// <summary>
/// A single ScriptableObject that handles dice slot restrictions in one place.
/// Supports restricting by color, by face value range, and by evens/odds.
/// </summary>
[CreateAssetMenu(fileName = "NewDiceSlotRestriction", menuName = "Dice/Slot Restriction")]
public class DiceSlotRestrictionSO : ScriptableObject
{
    [Header("Color Restriction")]
    [Tooltip("Check this if you want to restrict by specific colors.")]
    public bool restrictByColor;
    [Tooltip("Allowed colors if restrictByColor is true.")]
    public DiceColorSO[] allowedColors;

    [Header("Value Restriction")]
    [Tooltip("Set this if you want a minimum and maximum face value.")]
    public bool restrictByValueRange;
    public int minValue = 1;
    public int maxValue = 6;

    [Header("Even/Odd Restriction")]
    [Tooltip("Only allow even (2,4,6...) face values.")]
    public bool allowEvensOnly;
    [Tooltip("Only allow odd (1,3,5...) face values.")]
    public bool allowOddsOnly;

    /// <summary>
    /// Returns true if the dice meets all the enabled restrictions.
    /// </summary>
    /// <param name="dice">The data object for the dice we're testing.</param>
    /// <returns>True if the dice is allowed, false otherwise.</returns>
    public bool CheckDice(Dice dice)
    {
        // 1) Check color
        if (restrictByColor)
        {
            if (allowedColors == null || allowedColors.Length == 0)
            {
                // No colors in the list, but user checked "restrictByColor"? This fails or passes?
                // We'll assume it fails for clarity, because no colors means "not allowed at all."
                return false;
            }

            bool colorMatch = false;
            foreach (var colorSo in allowedColors)
            {
                if (dice.LogicalColor == colorSo)
                {
                    colorMatch = true;
                    break;
                }
            }

            if (!colorMatch)
            {
                return false;
            }
        }

        // 2) Check face value range
        if (restrictByValueRange)
        {
            if (dice.CurrentValue < minValue || dice.CurrentValue > maxValue)
            {
                return false;
            }
        }

        // 3) Check even/odd only
        // If the user sets both allowEvensOnly and allowOddsOnly to true, that's contradictory.
        // We can handle that edge case by deciding it always fails, or always passes. 
        // For example, let's make it fail if both are set:
        if (allowEvensOnly && allowOddsOnly)
        {
            // Contradictory; no dice can be both even and odd.
            return false;
        }
        if (allowEvensOnly && (dice.CurrentValue % 2 != 0))
        {
            return false;
        }
        if (allowOddsOnly && (dice.CurrentValue % 2 != 1))
        {
            return false;
        }

        // If we passed all checks, return true
        return true;
    }
}
