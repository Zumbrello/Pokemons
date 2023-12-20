package com.example.pokmob;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import androidx.appcompat.app.AppCompatActivity;

import calls.GetUser;
import calls.GetUserMobileAccount;
import models.Pokemon;
import retrofit.APIInterface;
import retrofit.PokemonsAPI;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {

    Button loginButton, exitButton;
    EditText loginText, passwordText;
    Retrofit retrofit;
    APIInterface api;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //Инициализация переменных, связанных с активити
        loginButton = (Button)findViewById(R.id.loginBtn);
        exitButton = (Button)findViewById(R.id.exitBtn);
        loginText = (EditText)findViewById(R.id.loginEdit);
        passwordText = (EditText)findViewById(R.id.passwordEdit);

        //Установка обработчиков нажатия
        loginButton.setOnClickListener(this);
        exitButton.setOnClickListener(this);

        //Настройка ретрофита
        retrofit = PokemonsAPI.getClient();
        api = retrofit.create(APIInterface.class);

        SharedPreferences myPreferences
                = MainActivity.this.getSharedPreferences("settings", Context.MODE_PRIVATE);

        //Автозаполнение полей, если ранее был произведён вход
        if(myPreferences.getString("PASSWORD", "") != "" &&
                myPreferences.getString("LOGIN", "") != ""){
            loginText.setText(myPreferences.getString("LOGIN", ""));
            passwordText.setText(myPreferences.getString("PASSWORD", ""));
        }
    }

    @Override
    public void onClick(View view) {

        if (view.getId() == R.id.loginBtn){

            //Запрос на получение токена (Авторизация)
            Call<GetUser.GetToken> jwt = api.createJWT(
                    loginText.getText().toString(),
                    passwordText.getText().toString()
            );

            Callback<GetUser.GetToken> jwtCall = new Callback<GetUser.GetToken>() {
                @Override
                public void onResponse(Call<GetUser.GetToken> call, retrofit2.Response<GetUser.GetToken> response) {
                    if (response.isSuccessful())
                    {
                        GetUser.GetToken apiResponse = response.body();
                        if (apiResponse.getError() == null){
                            SharedPreferences myPreferences
                                    = MainActivity.this.getSharedPreferences("settings", Context.MODE_PRIVATE);
                            SharedPreferences.Editor editor = myPreferences.edit();
                            editor.putString("TOKEN", response.body().getToken());
                            editor.putString("REFRESH_TOKEN", response.body().getRefreshToken());
                            editor.putString("LOGIN", MainActivity.this.loginText.getText().toString());
                            editor.putString("PASSWORD", MainActivity.this.passwordText.getText().toString());
                            editor.commit();
                        }
                    }else{
                        MainActivity.this.loginText.setText("");
                        MainActivity.this.passwordText.setText("");
                    }
                }

                @Override
                public void onFailure(Call<GetUser.GetToken> call, Throwable t) {
                    System.out.println("Network Error :: " + t.getLocalizedMessage());
                }
            };

            jwt.enqueue(jwtCall);

            //Запрос на получение данных об аккаунте пользователя
            MainActivity context = MainActivity.this;
            SharedPreferences myPreferences
                    = context.getSharedPreferences("settings", Context.MODE_PRIVATE);

            if(myPreferences.getString("TOKEN", "") != ""){

                Call<GetUserMobileAccount> accountData = api.GetUserMobileAccount(
                        myPreferences.getString("LOGIN", ""),
                        "Bearer " + myPreferences.getString("TOKEN", "").replace("\n", "")
                );
                Callback<GetUserMobileAccount> accountCall = new Callback<GetUserMobileAccount>() {
                    @Override
                    public void onResponse(Call<GetUserMobileAccount> call, Response<GetUserMobileAccount> response) {
                        if(response.isSuccessful()){
                            GetUserMobileAccount apiResponse = response.body();
                            try{
                                //Установка данных, которые будут переданы в следующую активити
                                Intent intent = new Intent(MainActivity.this, AccountActivity.class);
                                intent.putExtra("email", apiResponse.getEmail());
                                Pokemon pokemon = apiResponse.getPokemonNavigation();
                                if(pokemon != null) {
                                    intent.putExtra("pokemonImage", pokemon.getImage());
                                    intent.putExtra("pokemonTitle", apiResponse.getPokemonNavigation().getTitle());
                                }else{
                                    intent.putExtra("pokemonImage", "https://img.playbuzz.com/image/upload/ar_1.5,c_pad,f_jpg,b_auto/cdn/0b51319a-1441-4c2c-8269-b275c9fbebec/80ab71e5-67d6-4c70-a527-cee7f1afc31d.jpg");
                                    intent.putExtra("pokemonTitle", "Что это за покемон ?");
                                }
                                intent.putExtra("login", apiResponse.getLogin());
                                MainActivity.this.startActivity(intent);
                                MainActivity.this.finish();
                            }catch (Exception ex){
                                SharedPreferences myPreferences
                                        = context.getSharedPreferences("settings", Context.MODE_PRIVATE);
                                SharedPreferences.Editor editor = myPreferences.edit();
                                editor.remove("TOKEN");
                                editor.commit();
                                System.out.println(ex.getMessage());
                            }

                        }
                    }

                    @Override
                    public void onFailure(Call<GetUserMobileAccount> call, Throwable t) {
                        System.out.println("Network Error :: " + t.getLocalizedMessage());
                    }
                };
                accountData.enqueue(accountCall);
            }

        } else if(view.getId() == R.id.exitBtn){
            System.exit(0);
        }
    }
}