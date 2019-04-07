using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Sitecore.Framework.Rules;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Segmentation.Predicates;

namespace Sitecore.Support.XConnect.Segmentation.Predicates.Contacts
{
  public class MostRecentInteractionsOccurredAfter : ICondition, IMappableRuleEntity, IContactSearchQueryFactory
  {
    public DateTime Date { get; set; }

    [SuppressMessage("Data Flow", "SC1062:ValidateArgumentsOfPublicMethods", MessageId = "0#")]
    public bool Evaluate(IRuleExecutionContext context)
    {
      var c = context.Fact<Contact>(null);
      if (c.EngagementMeasures() == null)
        return false;
      return c.EngagementMeasures().MostRecentInteractionStartDateTime >= Date;
    }

    public Expression<Func<Contact, bool>> CreateContactSearchQuery(
      IContactSearchQueryContext context)
    {
      return contact => contact.EngagementMeasures().MostRecentInteractionStartDateTime >= Date.ToUniversalTime();
    }
  }
}