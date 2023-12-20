package models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

//Модель ответа при запросе данных об аккаунте пользователя
public class UserMobileAccountData {
    @SerializedName("Id")
    @Expose
    private Integer id;
    @SerializedName("User")
    @Expose
    private Integer user;
    @SerializedName("Login")
    @Expose
    private String login;
    @SerializedName("Email")
    @Expose
    private String email;
    @SerializedName("Pokemon")
    @Expose
    private String pokemon;
    @SerializedName("PokemonNavigation")
    @Expose
    private Pokemon pokemonNavigation;
    @SerializedName("UserNavigation")
    @Expose
    private User userNavigation;

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getUser() {
        return user;
    }

    public void setUser(Integer user) {
        this.user = user;
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPokemon() {
        return pokemon;
    }

    public void setPokemon(String pokemon) {
        this.pokemon = pokemon;
    }

    public Pokemon getPokemonNavigation() {
        return pokemonNavigation;
    }

    public void setPokemonNavigation(Pokemon pokemonNavigation) {
        this.pokemonNavigation = pokemonNavigation;
    }

    public User getUserNavigation() {
        return userNavigation;
    }

    public void setUserNavigation(User userNavigation) {
        this.userNavigation = userNavigation;
    }
}
