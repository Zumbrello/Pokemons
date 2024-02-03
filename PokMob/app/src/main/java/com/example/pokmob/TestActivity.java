package com.example.pokmob;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.RadioGroup;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import calls.GetAchivment;
import retrofit.APIInterface;
import retrofit.PokemonsAPI;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Retrofit;

public class TestActivity extends AppCompatActivity implements View.OnClickListener {

    Button answerBtn;
    RadioGroup attractRG, characterRG, importantCharacterRG, weatherRG;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_test);

        answerBtn = (Button) findViewById(R.id.send_answer);
        answerBtn.setOnClickListener(this);

        attractRG = (RadioGroup) findViewById(R.id.attract);
        characterRG = (RadioGroup) findViewById(R.id.character_group);
        importantCharacterRG = (RadioGroup) findViewById(R.id.important_character_group);
        weatherRG = (RadioGroup) findViewById(R.id.weather_group);
    }

    @Override
    public void onClick(View view) {
        if(view.getId() == R.id.send_answer){
            int attract = 0, correction = 0;

            if(attractRG.getCheckedRadioButtonId() == R.id.water){
                attract = 2;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.fire){
                attract = 11;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.earth){
                attract = 5;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.rock){
                attract = 6;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.ice){
                attract = 7;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.flying){
                attract = 8;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.ghost){
                attract = 12;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.psycho){
                attract = 13;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.grass){
                attract = 16;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.electro){
                attract = 17;
            }else if(attractRG.getCheckedRadioButtonId() == -1){
                Toast.makeText(this, "Заполните все ответы", Toast.LENGTH_LONG).show();
                return;
            }

            if(characterRG.getCheckedRadioButtonId() == R.id.evening){
                correction += 1;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.day){
                correction += 2;
            }else if(attractRG.getCheckedRadioButtonId() == R.id.morning){
                correction += 0;
            }else if(attractRG.getCheckedRadioButtonId() == -1){
                Toast.makeText(this, "Заполните все ответы", Toast.LENGTH_LONG).show();
                return;
            }

            if(importantCharacterRG.getCheckedRadioButtonId() == R.id.power){
                correction += 2;
            }else if(importantCharacterRG.getCheckedRadioButtonId() == R.id.intelligence){
                correction += 0;
            }else if(importantCharacterRG.getCheckedRadioButtonId() == R.id.chemistry){
                correction += 3;
            }else if(importantCharacterRG.getCheckedRadioButtonId() == R.id.physics){
                correction += 1;
            }else if(importantCharacterRG.getCheckedRadioButtonId() == R.id.humanity){
                correction += 4;
            }else if(importantCharacterRG.getCheckedRadioButtonId() == R.id.religion){
                correction += 5;
            }else if(importantCharacterRG.getCheckedRadioButtonId() == -1){
                Toast.makeText(this, "Заполните все ответы", Toast.LENGTH_LONG).show();
                return;
            }

            if(weatherRG.getCheckedRadioButtonId() == R.id.groza){
                correction += 3;
            }else if(weatherRG.getCheckedRadioButtonId() == R.id.rain){
                correction += 2;
            }else if(weatherRG.getCheckedRadioButtonId() == R.id.sun){
                correction += 0;
            }else if(weatherRG.getCheckedRadioButtonId() == R.id.pasmurno){
                correction += 1;
            }else if(weatherRG.getCheckedRadioButtonId() == -1){
                Toast.makeText(this, "Заполните все ответы", Toast.LENGTH_LONG).show();
                return;
            }

            //Настройка ретрофита
            Retrofit retrofit = PokemonsAPI.getClient();
            APIInterface api = retrofit.create(APIInterface.class);

            SharedPreferences myPreferences
                    = TestActivity.this.getSharedPreferences("settings", Context.MODE_PRIVATE);

            //Запрос на изменение значка
            Call<GetAchivment> achivment = api.GetAchivment(
                    myPreferences.getString("LOGIN", "").toString(),
                    attract,
                    correction,
                    "Bearer " + myPreferences.getString("TOKEN", "")
            );

            Callback<GetAchivment> achivmentCall = new Callback<GetAchivment>() {
                @Override
                public void onResponse(Call<GetAchivment> call, retrofit2.Response<GetAchivment> response) {
                    Bundle extras = getIntent().getExtras();
                    Intent intent = new Intent(TestActivity.this, AccountActivity.class);
                    intent.putExtra("login", extras.getString("login"));
                    intent.putExtra("email", extras.getString("email"));
                    intent.putExtra("pokemonTitle", response.body().getTitle());
                    intent.putExtra("pokemonImage", response.body().getImage());
                    TestActivity.this.startActivity(intent);
                    TestActivity.this.finish();
                }

                @Override
                public void onFailure(Call<GetAchivment> call, Throwable t) {
                    System.out.println("Network Error :: " + t.getLocalizedMessage());
                }
            };
            achivment.enqueue(achivmentCall);
        }
    }
}