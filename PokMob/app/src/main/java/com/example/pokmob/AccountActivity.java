package com.example.pokmob;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.squareup.picasso.Picasso;

public class AccountActivity extends AppCompatActivity implements View.OnClickListener{

    TextView pokemonText, emailText, loginText;
    ImageView pokemonImage;
    Button LogoutBtn, StartTest;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_account);
    }

    @Override
    protected void onResume() {
        super.onResume();

        //Инициализация переменных, связанных с активити
        pokemonText = findViewById(R.id.pokemonText);
        emailText = findViewById(R.id.emailText);
        loginText = findViewById(R.id.loginText);
        pokemonImage = findViewById(R.id.pokemonImage);
        LogoutBtn = findViewById(R.id.logoutBtn);
        StartTest = findViewById(R.id.startTestBtn);

        //Установка обработчиков нажатия
        StartTest.setOnClickListener(this);
        LogoutBtn.setOnClickListener(this);

        //Установка данных в полях активити
        Bundle extras = getIntent().getExtras();
        SharedPreferences myPreferences
                = AccountActivity.this.getSharedPreferences("settings", Context.MODE_PRIVATE);

        if (extras != null) {
            emailText.setText(extras.getString("email"));
            loginText.setText(extras.getString("login"));
            pokemonText.setText(extras.getString("pokemonTitle"));
            Picasso.get().load(extras.getString("pokemonImage")).into(pokemonImage);
        }
    }

    public AccountActivity(){
    }

    @Override
    public void onClick(View view) {
        if (view.getId() == R.id.logoutBtn){
                        Intent intent = new Intent(AccountActivity.this, MainActivity.class);
            startActivity(intent);
            AccountActivity.this.finish();
        }else if(view.getId() == R.id.startTestBtn){
            Bundle extras = getIntent().getExtras();
            SharedPreferences myPreferences
                    = AccountActivity.this.getSharedPreferences("settings", Context.MODE_PRIVATE);
            Intent intent = new Intent(AccountActivity.this, TestActivity.class);
            intent.putExtra("email", extras.getString("email"));
            intent.putExtra("login", myPreferences.getString("LOGIN", ""));
            startActivity(intent);
            AccountActivity.this.finish();
        }
    }
}
