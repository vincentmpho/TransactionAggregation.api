using TransactionAggregation.Application.Abstractions;
using TransactionAggregation.Domain.Entities;
using TransactionAggregation.Domain.Enums;

namespace TransactionAggregation.Application.Services;

// Assigns a category to a transaction by matching keywords found in its description or merchant name.
public class CategorizationEngine : ICategorizationEngine
{
    // Each category maps to a list of keywords that identify it.
    private static readonly Dictionary<TransactionCategory, string[]> Keywords = new()
    {
        [TransactionCategory.Groceries] = new[] { "checkers", "woolworths", "pick n pay", "spar", "shoprite" },
        [TransactionCategory.Transport] = new[] { "uber", "bolt", "petrol", "fuel", "engen", "shell", "gautrain" },
        [TransactionCategory.Utilities] = new[] { "eskom", "electricity", "water", "vodacom", "mtn", "telkom", "airtime" },
        [TransactionCategory.Rent] = new[] { "rent", "landlord", "lease" },
        [TransactionCategory.DiningOut] = new[] { "restaurant", "kfc", "mcdonald", "nando", "steers", "coffee" },
        [TransactionCategory.Entertainment] = new[] { "netflix", "showmax", "spotify", "dstv", "cinema" },
        [TransactionCategory.Health] = new[] { "pharmacy", "clicks", "dischem", "doctor", "hospital" },
        [TransactionCategory.Insurance] = new[] { "insurance", "outsurance", "santam", "premium" },
        [TransactionCategory.Shopping] = new[] { "takealot", "amazon", "mr price", "game", "makro" },
        [TransactionCategory.Savings] = new[] { "savings", "investment" },
        [TransactionCategory.Fees] = new[] { "fee", "charge" },
        [TransactionCategory.Income] = new[] { "salary", "wage", "payroll" },
        [TransactionCategory.Transfers] = new[] { "transfer", "eft", "payment to" }
    };

    public void Categorize(Transaction transaction)
    {
        // Combine description and merchant name into one lowercase string to search.
        var text = $"{transaction.Description} {transaction.MerchantName}".ToLowerInvariant();

        foreach (var pair in Keywords)
        {
            foreach (var keyword in pair.Value)
            {
                if (text.Contains(keyword))
                {
                    transaction.Category = pair.Key;
                    return;
                }
            }
        }

        // If nothing matched but money came in, treat it as Income.
        if (transaction.Type == TransactionType.Credit)
        {
            transaction.Category = TransactionCategory.Income;
            return;
        }

        // Otherwise leave it Uncategorized.
        transaction.Category = TransactionCategory.Uncategorized;
    }
}


