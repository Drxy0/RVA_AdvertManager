using System.Collections.Generic;

// TODO: Note - Mby rename 'Invoker' to something smarter 'ChangesManager', 'HistoryManager' etc.
// Mby also put it in another namespace/folder. idk
namespace AdvertManager.Domain.Command
{
	public class Invoker
	{
		private Stack<AdvertisementCommand> undoStack;
		private Stack<AdvertisementCommand> redoStack;

		public void ExecuteCommand(AdvertisementCommand command)
		{
			command.Execute();
		}
	}
}
