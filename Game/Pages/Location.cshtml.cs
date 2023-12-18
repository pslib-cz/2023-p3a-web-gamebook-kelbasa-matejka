using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Game.Services;
using Microsoft.AspNetCore.Mvc;


namespace Game.Pages;

public class Location : PageModel
{
    private readonly LocationService LocSer;
    private readonly ISessionService SessionSer;
    private readonly EffectService EffSer;
    private readonly PlayerService PlayerSer;

    private static readonly string PLAYER = "PlayerSessionKey";
    private static readonly string VISITED_CONNECTIONS = "VisitedConnectionsSessionKey";
    
    public Location(LocationService ls, ISessionService ss, EffectService es, PlayerService ps)
    {
        LocSer = ls;
        SessionSer = ss;
        EffSer = es;
        PlayerSer = ps;        
    }
    public LocationModel lModel { get; private set; }
    public PlayerModel pModel { get; set; }
    
    
    public void OnGet(int id)
    {
        pModel = SessionSer.GetSession<PlayerModel>(PLAYER);
        Console.WriteLine("start: " + pModel.VisitedConnections);
        int last = pModel.CurrentLocationId;
        ConnectionModel con;
        if (id != 0 && last != 0 && LocSer.ExistsLocation(id))
        {
            if (LocSer.IsNavigationLegitimate(last, id))
            {
                /*
                 * TODO Connection required items check
                 */
                con = LocSer.GetConnections(last).Where(a => a.ToLocationID == id).FirstOrDefault();
                ;
                if(con.Effect != null && !PlayerSer.ConnectionWasUsed(pModel, con.FromLocationID, con.ToLocationID))
                {
                    EffSer.ApplyEffect(con.Effect, pModel);
                    PlayerSer.SaveUsedConnection(pModel, con.FromLocationID, con.ToLocationID);
                    Console.WriteLine(pModel.VisitedConnections);
                }
                pModel.CurrentLocationId = id;
            }

        }
        
        SessionSer.SaveSession<PlayerModel>(PLAYER, pModel);
        lModel = LocSer.GetLocation(pModel.CurrentLocationId);
    }
}