using UnityEngine;

/// <summary>
/// A single ScriptableObject that handles dice slot restrictions in one place.
/// Supports restrictions by color, by face value range, evens/odds, 
/// single value, exclude value/color, and sum requirement.
/// </summary>
[CreateAssetMenu(fileName = "NewDiceSlotRestriction", menuName = "Dice/Slot Restriction")]
public class DiceSlotRestrictionSO : ScriptableObject
{
    [Header("Base Restrictions")]
    [Tooltip("If true, accepts any type of dice regardless of other restrictions.")]
    public bool allowAnyDice = false;

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

    [Header("Additional Restrictions")]
    [Tooltip("Check this to only allow a single specific face value.")]
    public bool restrictToSingleValue;
    [Tooltip("The single face value that is allowed if restrictToSingleValue is true.")]
    public int requiredValue;

    [Tooltip("Check this to exclude a specific face value.")]
    public bool excludeValue;
    [Tooltip("The face value to exclude if excludeValue is true.")]
    public int valueToExclude;

    [Tooltip("Check this to exclude specific colors.")]
    public bool excludeColor;
    [Tooltip("Colors to exclude if excludeColor is true.")]
    public DiceColorSO[] colorsToExclude;

    [Header("Sum of Values Restriction")]
    [Tooltip("Check this if you want the dice to meet a sum requirement (for single dice, this is effectively a single value requirement).")]
    public bool restrictBySumRequirement;
    [Tooltip("The required sum that the dice value should meet.")]
    public int requiredSum;

    /// <summary>
    /// Returns true if the dice meets all the enabled restrictions.
    /// </summary>
    /// <param name="dice">The data object for the dice we're testing.</param>
    /// <returns>True if the dice is allowed, false otherwise.</returns>
    public bool CheckDice(Dice dice)
    {
        // 0) If allowAnyDice is true, accept regardless of other restrictions.
        if (allowAnyDice)
        {
            return true;
        }

        // 1) Check exclude color with an array of colors to exclude.
        if (excludeColor)
        {
            if (colorsToExclude != null && colorsToExclude.Length > 0)
            {
                foreach (var color in colorsToExclude)
                {
                    if (dice.LogicalColor == color)
                    {
                        // Dice color is in the exclusion list.
                        return false;
                    }
                }
            }
            else
            {
                // If excludeColor is enabled but no colors specified, reject all dice.
                return false;
            }
        }

        // 2) Check color restrictions.
        if (restrictByColor)
        {
            if (allowedColors == null || allowedColors.Length == 0)
            {
                // No colors specified, fail the check.
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

        // 3) Check single value restriction.
        if (restrictToSingleValue)
        {
            if (dice.CurrentValue != requiredValue)
            {
                return false;
            }
        }

        // 4) Check exclude value.
        if (excludeValue)
        {
            if (dice.CurrentValue == valueToExclude)
            {
                return false;
            }
        }

        // 5) Check face value range.
        if (restrictByValueRange)
        {
            if (dice.CurrentValue < minValue || dice.CurrentValue > maxValue)
            {
                return false;
            }
        }

        // 6) Check even/odd only.
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

        // If all checks passed, the dice is allowed.
        return true;
    }
}
