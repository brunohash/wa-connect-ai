const { create, Client } = require('@open-wa/wa-automate');
const messageHandler = require('./messageHandler').default;
const options = require('./config/options');

class WhatsAppService {

  constructor(client) {
    this.client = client;
    this.initialize();
  }

  initialize() {
    console.log('[SERVER] Servidor iniciado!');
    this.client.onStateChanged(this.handleStateChange);
    this.client.onMessage(this.handleMessage);
  }

  handleStateChange(state) {
    console.log('[Status do cliente]', state);
    if (state === 'CONFLICT' || state === 'UNLAUNCHED') {
      this.client.forceRefocus();
    }
  }

  async handleMessage(message) {
    try {
      await this.manageMessageCache();
      messageHandler(this.client, message);
    } catch (error) {
      console.error('Erro ao processar a mensagem:', error);
    }
  }

  async manageMessageCache() {
    const loadedMessages = await this.client.getAmountOfLoadedMessages();
    if (loadedMessages >= 3000) {
      this.client.cutMsgCache();
    }
  }
}

const start = async (client = new Client()) => {
  new WhatsAppService(client);
};

create(options(true, start))
  .then(client => start(client))
  .catch(error => console.log(error));