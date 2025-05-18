using System.Collections.Concurrent;
using System.ComponentModel;

namespace LLQE.Common.Services
{
    public class NodeMessagesStore : INotifyPropertyChanged
    {
        private readonly ConcurrentDictionary<string, string> _messages = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public string GetMessage(string nodeName) => _messages.TryGetValue(nodeName, out var msg) ? msg : string.Empty;

        public void SetMessage(string nodeName, string message)
        {
            _messages[nodeName] = message;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nodeName));
        }
    }
}
