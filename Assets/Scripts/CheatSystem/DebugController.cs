using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
//#if DEVELOPMENT_BUILD
    bool showConsole;
    bool showHelp;
    string input;

    public static DebugCommand KILL_ALL;
    public static DebugCommand HELP;

    public List<object> commandList;

    private void Awake()
    {
        KILL_ALL = new DebugCommand("kill_all", "Kill all the monsters from the scene.", "kill_all", () =>
        {
            Debug.Log("Killing everybody");
        });

        HELP = new DebugCommand("Help", "Shows a list of commands", "help", () =>
        {
            showHelp = true;
        });

        commandList = new List<object>
        {
            KILL_ALL,
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

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            for(int i=0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommand;

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

        for(int i=0; i<commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }else if(commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
            }
        }
    }
//#endif
}
