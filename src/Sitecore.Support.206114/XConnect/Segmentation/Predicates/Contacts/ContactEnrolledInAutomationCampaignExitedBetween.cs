using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.Framework.Rules;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Segmentation.Predicates;

namespace Sitecore.Support.XConnect.Segmentation.Predicates.Contacts
{
  public class ContactEnrolledInAutomationCampaignExitedBetween : ICondition, IMappableRuleEntity,
    IContactSearchQueryFactory
  {
    public Guid AutomationCampaignId { get; set; }

    public DateTime MinDate { get; set; }

    public DateTime MaxDate { get; set; }

    [SuppressMessage("Data Flow", "SC1062:ValidateArgumentsOfPublicMethods", MessageId = "0#")]
    public bool Evaluate(IRuleExecutionContext context)
    {
      var c = context.Fact<Contact>(null);
      if (c.AutomationPlanExit() == null)
        return false;
      return c.AutomationPlanExit().Results.Where(e => e.AutomationPlanDefinitionId == AutomationCampaignId).Any(e =>
      {
        if (e.ExitDateTime >= MinDate)
        {
          return e.ExitDateTime <= MaxDate;
        }

        return false;
      });
    }

    public Expression<Func<Contact, bool>> CreateContactSearchQuery(
      IContactSearchQueryContext context)
    {
      return contact => contact.AutomationPlanExit().Results
        .Where(a => a.AutomationPlanDefinitionId == AutomationCampaignId)
        .Any(a => a.ExitDateTime >= MinDate && a.ExitDateTime <= MaxDate);
    }
  }
}