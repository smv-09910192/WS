import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BaseDto, ServerEchoClientDto } from '../BaseDto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'AClient';
  messages: string[] = [];
  ws: WebSocket = new WebSocket("ws://localhost:8181");
  messageContent: FormControl;    
  //messageForm: FormGroup;

  constructor() { 
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      //@ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);
    }
    this.messageContent = new FormControl('');
  }

  ServerEchoClient(dto: ServerEchoClientDto) {
    this.messages.push(dto.echoValue!)
  }

  sendMessage() {
    var object = {
      eventType: "ClientToServer",
      messageContent: this.messageContent.value!
    };
    this.ws.send(JSON.stringify(object));
  }
}
