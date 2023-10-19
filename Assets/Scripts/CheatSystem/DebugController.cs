using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private bool showConsole;
    private bool showHelp;
    private string input;

    public static DebugCommand<int> DRAW_CARDS;
    public static DebugCommand DRAW_NEW_HAND;
    public static DebugCommand KILL_ALL;
    public static DebugCommand<int> HEAL_PLAYER;
    public static DebugCommand<int> GIVE_ARMOR_PLAYER;
    public static DebugCommand<int> GAIN_GOLD;
    public static DebugCommand HELP;

    private List<object> commandList;

    private void Awake()
    {

        KILL_ALL = new DebugCommand("kill_all", "Kill all the enemies", "kill_all", () =>
        {
            for(int i = 0; i< BattleController.instance.enemies.Length;i++)
            {
                BattleController.instance.enemies[i].TakeDamage(2147483647);
            }
        });
        DRAW_CARDS = new DebugCommand<int>("draw_cards", "Draw a certain amount of cards", "Draw_card <card_amount>", (x) =>
        {
            DeckController.instance.DrawMultipleCards(x);
        });
        DRAW_NEW_HAND = new DebugCommand("draw_hand", "Draw a new hand", "draw_hand", () =>
        {
            HandController.instance.EmptyHand();
            DeckController.instance.DrawMultipleCards(5);
        });
        GAIN_GOLD = new DebugCommand<int>("gain_gold", "Gain a certain amount of gold", "gain_gold <gold_amount>", (x) =>
        {
            BattleController.instance.goldEarned += x;
        });
        HEAL_PLAYER = new DebugCommand<int>("heal_player", "Heal the player a certain amount", "heal_player <heal_amount>", (x) =>
        {
            Player.instance.GiveHealth(x);
        });
        GIVE_ARMOR_PLAYER = new DebugCommand<int>("armor_player", "Give armor to the player a certain amount", "armor_player <armor_amount>", (x) =>
        {
            Player.instance.GiveArmor(x);
        });
        HELP = new DebugCommand("help", "Show commands", "help", () =>
        {
            showHelp = !showHelp;
        });


        commandList = new List<object>
        {
            KILL_ALL,
            DRAW_CARDS,
            DRAW_NEW_HAND,
            GAIN_GOLD,
            HEAL_PLAYER,
            GIVE_ARMOR_PLAYER,
            HELP
        };
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleDebug"))
        {
            showConsole = !showConsole;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!showConsole) return;

            HandleInput();
            input = "";

        }
    }

    Vector2 scroll;

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");
            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} - {command.commandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();
            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }

                if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }

                if (commandList[i] as DebugCommand<string> != null)
                {
                    (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                }
            }
        }
    }
}
