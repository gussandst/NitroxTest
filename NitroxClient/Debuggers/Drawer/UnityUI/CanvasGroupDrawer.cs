﻿using UnityEngine;

namespace NitroxClient.Debuggers.Drawer.UnityUI;

public class CanvasGroupDrawer : IDrawer<CanvasGroup>
{
    public void Draw(CanvasGroup canvasGroup)
    {
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Alpha", NitroxGUILayout.DrawerLabel, GUILayout.Width(NitroxGUILayout.DEFAULT_LABEL_WIDTH));
            NitroxGUILayout.Separator();
            canvasGroup.alpha = NitroxGUILayout.SliderField(canvasGroup.alpha, 0f, 1f);
        }

        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Interactable", NitroxGUILayout.DrawerLabel, GUILayout.Width(NitroxGUILayout.DEFAULT_LABEL_WIDTH));
            NitroxGUILayout.Separator();
            canvasGroup.interactable = NitroxGUILayout.BoolField(canvasGroup.interactable);
        }

        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Blocks Raycasts", NitroxGUILayout.DrawerLabel, GUILayout.Width(NitroxGUILayout.DEFAULT_LABEL_WIDTH));
            NitroxGUILayout.Separator();
            canvasGroup.blocksRaycasts = NitroxGUILayout.BoolField(canvasGroup.blocksRaycasts);
        }

        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Ignore Parent Groups", NitroxGUILayout.DrawerLabel, GUILayout.Width(NitroxGUILayout.DEFAULT_LABEL_WIDTH));
            NitroxGUILayout.Separator();
            canvasGroup.ignoreParentGroups = NitroxGUILayout.BoolField(canvasGroup.ignoreParentGroups);
        }
    }
}
