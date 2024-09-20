
public class StorePresenter
{
    private LocalAndCloudDataService dataService;
    private StoreView view;

    public StorePresenter(LocalAndCloudDataService dataService)
    {
        this.dataService = dataService;
    }

    public void SetView(StoreView view)
    {
        this.view = view;
    }

    public void Initialize()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        //var items = dataService
        //view.DisplayItems(items);
        //view.UpdateMoney(dataService.GetCurrentMoney());
    }

}