package retrofit;

import calls.GetAchivment;
import calls.GetUser;
import calls.GetUserMobileAccount;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.Query;
//Интерфейс апи
public interface APIInterface {
    //Запрос на получение токена
    @POST("/Login/Login")
    Call<GetUser.GetToken> createJWT(
            @Query("login") String login,
            @Query("password") String password
    );

    //Запрос получения данных об аккаунте для телефона
    @GET("/GetUserMobileAccount")
    Call<GetUserMobileAccount> GetUserMobileAccount(
            @Query("login") String login,
            @Header("Authorization") String authHeader
    );

    //Запрос данных о достижении
    @GET("/GetAchivment")
    Call<GetAchivment> GetAchivment(
            @Query("login") String login,
            @Query("pokemonType") Integer pokemonType,
            @Query("correction") Integer correction,
            @Header("Authorization") String authHeader
    );
}
