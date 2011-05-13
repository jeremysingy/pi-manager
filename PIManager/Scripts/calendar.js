function UCCalendarToggleCalendar(panelId)
{
    // Get the panel to show/hide
    var pnl = document.getElementById(panelId);

    if (!pnl)
        return;

    // Show/hide the panel
    pnl.style.display = (pnl.style.display == "none" ? "block" : "none");
}