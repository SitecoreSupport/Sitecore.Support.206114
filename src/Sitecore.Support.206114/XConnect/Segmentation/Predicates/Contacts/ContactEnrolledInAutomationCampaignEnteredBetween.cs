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
  public class ContactEnrolledInAutomationCampaignEnteredBetween : ICondition, IMappableRuleEntity,
    IContactSearchQueryFactory
  {
    public Guid AutomationCampaignId { get; set; }

    public DateTime MinDate { get; set; }

    public DateTime MaxDate { get; set; }

    [SuppressMessage("Data Flow", "SC1062:ValidateArgumentsOfPublicMethods", MessageId = "0#")]
    public bool Evaluate(IRuleExecutionContext context)
    {
      var c = context.Fact<Contact>(null);
      if (c.AutomationPlanEnrollmentCache() != null)
        return c.AutomationPlanEnrollmentCache().ActivityEnrollments
          .Where(e => e.AutomationPlanDefinitionId == AutomationCampaignId).Any(e =>
          {
            if (e.ActivityEntryDate >= MinDate)
            {
              return e.ActivityEntryDate <= MaxDate;
            }

            return false;
          });
      return false;
    }

    public Expression<Func<Contact, bool>> CreateContactSearchQuery(
      IContactSearchQueryContext context)
    {
      return contact => contact.AutomationPlanEnrollmentCache().ActivityEnrollments
        .Where(e => e.AutomationPlanDefinitionId == AutomationCampaignId)
        .Any(e => e.ActivityEntryDate >= MinDate && e.ActivityEntryDate <= MaxDate);
    }
  }
}