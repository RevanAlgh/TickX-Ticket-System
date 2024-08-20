import { Injectable } from '@angular/core';
import { Observable, Subscriber } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FileReaderService {
  constructor() {}

  validateFile(file: File): Promise<boolean> {
    return new Promise((resolve, reject) => {
      if (file.type === 'application/pdf') {
        // PDF files are automatically valid
        resolve(true);
      } else if (['image/png', 'image/jpeg'].includes(file.type)) {
        const img = new Image();
        img.onload = () => {
          const width = img.width;
          const height = img.height;
          const megapixels = (width * height) / 1000000;
          resolve(megapixels <= 2);
        };
        img.onerror = (error) => {
          reject(error);
        };
        img.src = URL.createObjectURL(file);
      } else {
        resolve(false); // Unsupported file type
      }
    });
  }

  convertToBase64(file: File): Observable<string | ArrayBuffer | null> {
    return new Observable((subscriber: Subscriber<any>) => {
      this.readFile(file, subscriber);
    });
  }

  private readFile(file: File, subscriber: Subscriber<any>) {
    const fileReader = new FileReader();
    fileReader.readAsDataURL(file);

    fileReader.onload = () => {
      subscriber.next(fileReader.result);
      subscriber.complete();
    };
    fileReader.onerror = (error) => {
      subscriber.error(error);
      subscriber.complete();
    };
  }

  base64ToFile(base64String: string, filename: string, mimeType: string): File {
    const binaryString = window.atob(base64String); // Decode Base64 string
    const arrayBuffer = new ArrayBuffer(binaryString.length);
    const uint8Array = new Uint8Array(arrayBuffer);

    for (let i = 0; i < binaryString.length; i++) {
      uint8Array[i] = binaryString.charCodeAt(i);
    }

    return new File([arrayBuffer], filename, { type: mimeType });
  }
}
