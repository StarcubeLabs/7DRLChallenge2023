using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    [SerializeField]
    private float messageDuration;
    [SerializeField]
    private int maxMessages;
    
    private List<Message> messages = new List<Message>();
    
    private TextMeshProUGUI textBox;

    class Message
    {
        public string messageText;
        public float timeLeft;

        public Message(string messageText, float timeLeft)
        {
            this.messageText = messageText;
            this.timeLeft = timeLeft;
        }
    }
    
    private void Start()
    {
        textBox = GetComponent<TextMeshProUGUI>();
        textBox.enabled = false;
    }

    void Update()
    {
        List<Message> messagesToRemove = new List<Message>();
        foreach (Message message in messages)
        {
            message.timeLeft -= Time.deltaTime;
            if (message.timeLeft <= 0)
            {
                messagesToRemove.Add(message);
            }
        }
        
        messagesToRemove.ForEach(message => messages.Remove(message));
        if (messagesToRemove.Count > 0)
        {
            RedrawTextBox();
        }
    }

    public void AddMessage(string messageText)
    {
        messages.Add(new Message(messageText, messageDuration));
        if (messages.Count > maxMessages)
        {
            messages.RemoveAt(0);
        }
        RedrawTextBox();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        messages.Clear();
        RedrawTextBox();
    }

    private void RedrawTextBox()
    {
        List<string> messageTexts = messages.ConvertAll(message => message.messageText);
        string textBoxMessage = string.Join("\n", messageTexts);
        textBox.text = textBoxMessage;
        textBox.enabled = messages.Count > 0;
    }
}
