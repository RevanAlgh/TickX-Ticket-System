import { HttpClient } from '@angular/common/http';
import { ModuleWithProviders, NgModule, Provider } from '@angular/core';
import {
  TranslateCompiler,
  TranslateLoader,
  TranslateModule,
} from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateMessageFormatCompiler } from 'ngx-translate-messageformat-compiler';

//responsiple to locate the translation files
const httpLoaderFactory = (http: HttpClient) =>
  new TranslateHttpLoader(http, 'assets/i18n/', '.json');

const translateCompilerFactory = () => new TranslateMessageFormatCompiler();

const translateLoader: Provider = {
  provide: TranslateLoader,
  useFactory: httpLoaderFactory,
  deps: [HttpClient],
};

const translateCompiler: Provider = {
  provide: TranslateCompiler,
  useFactory: translateCompilerFactory,
};

@NgModule()
export class AppTranslateModule {
  static forRoot(): ModuleWithProviders<AppTranslateModule> {
    return TranslateModule.forRoot({
      loader: translateLoader,
      compiler: translateCompiler,
    });
  }

  static forChild(): ModuleWithProviders<AppTranslateModule> {
    return TranslateModule.forRoot({
      loader: translateLoader,
      compiler: translateCompiler,
      isolate: false,
    });
  }
}
