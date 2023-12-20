package calls;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

import models.Pokemon;

public class GetPokemonsByURL {
    @SerializedName("Result")
    @Expose
    private Boolean result;
    @SerializedName("data")
    @Expose
    private List<Pokemon> data;

    public Boolean getResult() {
        return result;
    }

    public void setResult(Boolean result) {
        this.result = result;
    }

    public List<Pokemon> getData() {
        return data;
    }

    public void setData(List<Pokemon> data) {
        this.data = data;
    }

}
