using System.Collections.Generic;

public class EventManager
{
	public static Queue<string> infos = new Queue<string>();

	public static void AddInfoEvent(string info)
	{
		infos.Enqueue(info);
	}
}
