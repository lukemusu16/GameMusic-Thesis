class Logger
{
    // Available log levels
	static levels = {
		info = 1,
		debug = 2
	};
	
	// Prefix text for all logs
	static log_prefix = "[ADV_STATS]";

	function info(message)
	{
		if (!(::ADV_STATS_LOG_LEVEL >= levels.info))
		    return;
		
		printl(log_prefix + "[INFO] " + message);
	}
	
	function debug(message, params = null)
	{
		if (!(::ADV_STATS_LOG_LEVEL >= levels.debug))
		    return;
			
		printl(log_prefix + "[DEBUG] " + message);
		
		if (!params)
		    return;

		g_ModeScript.DeepPrintTable(params);
	}
}