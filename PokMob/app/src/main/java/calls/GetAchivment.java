package calls;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class GetAchivment {

    @SerializedName("Url")
    @Expose
    private String url;
    @SerializedName("Title")
    @Expose
    private String title;
    @SerializedName("Number")
    @Expose
    private Integer number;
    @SerializedName("Image")
    @Expose
    private String image;
    @SerializedName("Height")
    @Expose
    private Double height;
    @SerializedName("Weight")
    @Expose
    private Double weight;
    @SerializedName("ExpGroup")
    @Expose
    private Integer expGroup;
    @SerializedName("Health")
    @Expose
    private Integer health;
    @SerializedName("Attack")
    @Expose
    private Integer attack;
    @SerializedName("Protection")
    @Expose
    private Integer protection;
    @SerializedName("SpecialAttack")
    @Expose
    private Integer specialAttack;
    @SerializedName("SpecialProtection")
    @Expose
    private Integer specialProtection;
    @SerializedName("Speed")
    @Expose
    private Integer speed;
    @SerializedName("Summary")
    @Expose
    private Integer summary;
    @SerializedName("Male")
    @Expose
    private Double male;
    @SerializedName("Female")
    @Expose
    private Double female;
    @SerializedName("HatchingPeriod")
    @Expose
    private Integer hatchingPeriod;
    @SerializedName("Rate")
    @Expose
    private Double rate;
    @SerializedName("Expired")
    @Expose
    private Object expired;
    @SerializedName("ExpiredType")
    @Expose
    private Object expiredType;

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public Integer getNumber() {
        return number;
    }

    public void setNumber(Integer number) {
        this.number = number;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }

    public Double getHeight() {
        return height;
    }

    public void setHeight(Double height) {
        this.height = height;
    }

    public Double getWeight() {
        return weight;
    }

    public void setWeight(Double weight) {
        this.weight = weight;
    }

    public Integer getExpGroup() {
        return expGroup;
    }

    public void setExpGroup(Integer expGroup) {
        this.expGroup = expGroup;
    }

    public Integer getHealth() {
        return health;
    }

    public void setHealth(Integer health) {
        this.health = health;
    }

    public Integer getAttack() {
        return attack;
    }

    public void setAttack(Integer attack) {
        this.attack = attack;
    }

    public Integer getProtection() {
        return protection;
    }

    public void setProtection(Integer protection) {
        this.protection = protection;
    }

    public Integer getSpecialAttack() {
        return specialAttack;
    }

    public void setSpecialAttack(Integer specialAttack) {
        this.specialAttack = specialAttack;
    }

    public Integer getSpecialProtection() {
        return specialProtection;
    }

    public void setSpecialProtection(Integer specialProtection) {
        this.specialProtection = specialProtection;
    }

    public Integer getSpeed() {
        return speed;
    }

    public void setSpeed(Integer speed) {
        this.speed = speed;
    }

    public Integer getSummary() {
        return summary;
    }

    public void setSummary(Integer summary) {
        this.summary = summary;
    }

    public Double getMale() {
        return male;
    }

    public void setMale(Double male) {
        this.male = male;
    }

    public Double getFemale() {
        return female;
    }

    public void setFemale(Double female) {
        this.female = female;
    }

    public Integer getHatchingPeriod() {
        return hatchingPeriod;
    }

    public void setHatchingPeriod(Integer hatchingPeriod) {
        this.hatchingPeriod = hatchingPeriod;
    }

    public Double getRate() {
        return rate;
    }

    public void setRate(Double rate) {
        this.rate = rate;
    }

    public Object getExpired() {
        return expired;
    }

    public void setExpired(Object expired) {
        this.expired = expired;
    }

    public Object getExpiredType() {
        return expiredType;
    }

    public void setExpiredType(Object expiredType) {
        this.expiredType = expiredType;
    }

}
