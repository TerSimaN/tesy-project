namespace Tesy.Commands
{
    public class MyMessages
    {
        private string contentToWrite = "";
        private readonly TesyHttpClient tesyHttpClient;
        private readonly StreamDeserializer deserializer = new();
        private readonly TesyFileEditor tesyFileEditor = new();
        private Dictionary<string, string> inputQueryParams = new();

        public MyMessages(TesyHttpClient tesyHttpClient)
        {
            this.tesyHttpClient = tesyHttpClient;
        }

        public async void GetMyMessages()
        {
            HttpResponseMessage responseMessage = tesyHttpClient.Get(TesyConstants.MyMessagesUrl, inputQueryParams);
            Stream stream = responseMessage.Content.ReadAsStream();
            string responseMessageContent = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessageContent.Contains("error"))
            {
                var noMatchFoundInRecordsErrorResponse = deserializer.GetNoMatchFoundInRecordsError(stream);
                contentToWrite = ContentBuilder.BuildNoMatchFoundInRecordsErrorString(noMatchFoundInRecordsErrorResponse);
            }
            else
            {
                contentToWrite = $"MyMessagesResponse: {responseMessageContent}\n\n";
            }
            tesyFileEditor.WriteToFile(TesyConstants.PathToHttpResponseMessagesFile, contentToWrite);
        }
    }
}