const { post } = require("axios");
const { Agent } = require("https");

const httpsAgent = new Agent({ rejectUnauthorized: false });

const sendMessageToAPI = async (messageContent) => {
  try {
    const response = await post('https://localhost:7131/Generic', {
      message: messageContent
    }, {
      httpsAgent
    });
    return response.data.message;
  } catch (error) {
    console.error("Erro ao enviar mensagem para a API:", error);
    throw error;
  }
};

module.exports = async (client, message) => {
  try {
    const { body, chat } = message;

    const apiMessage = await sendMessageToAPI(body);

    await client.sendText(chat.id, apiMessage);
  } catch (err) {
    console.error("Erro:", err);
    await client.sendText(chat.id, `Erro: ${err.message}`);
  }
};