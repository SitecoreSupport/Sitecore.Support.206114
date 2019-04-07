using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.SecurityModel;

namespace Sitecore.Support
{
  public class ChangeConditionItems
  {
    private readonly Database _database;

    public ChangeConditionItems()
    {
      _database = Database.GetDatabase("master");
    }

    public void Process(PipelineArgs args)
    {
      ChangeConditionType(new ID("{88A1F6BD-896A-483F-AAD7-1E84A065453A}"),"Sitecore.Support.XConnect.Segmentation.Predicates.Contacts.ContactEnrolledInAutomationCampaignEnteredBetween, Sitecore.Support.206114");
      ChangeConditionType(new ID("{7A9906C6-D85C-433F-962F-233C0561248B}"),"Sitecore.Support.XConnect.Segmentation.Predicates.Contacts.ContactEnrolledInAutomationCampaignExitedBetween, Sitecore.Support.206114");
      ChangeConditionType(new ID("{B92F4D18-D059-4E6A-837C-FE6E8A16C241}"),"Sitecore.Support.XConnect.Segmentation.Predicates.Contacts.MostRecentInteractionsOccurredAfter, Sitecore.Support.206114");
      ChangeConditionType(new ID("{658DD8E6-5FA5-4628-914C-ADE5A942C627}"),"Sitecore.Support.XConnect.Segmentation.Predicates.Contacts.MostRecentInteractionsOccurredBefore, Sitecore.Support.206114");
    }

    private void ChangeConditionType(ID itemId, string fieldValue)
    {
      if (_database == null)
      {
        Log.Warn("Sitecore.Support.206114 can't retrieve master database.", this);
        return;
      }

      var item = _database.GetItem(itemId);

      if (item == null)
      {
        Log.Warn($"Sitecore.Support.206114 can't retrieve item {itemId}", this);
        return;
      }

      var currentFieldValue = item["Type"];
      if (currentFieldValue != fieldValue)
      {
        using (new EditContext(item, SecurityCheck.Disable))
        {
          item["Type"] = fieldValue;
        }
      }
    }
  }
}