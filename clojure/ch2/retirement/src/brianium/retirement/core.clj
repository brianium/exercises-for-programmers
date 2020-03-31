(ns brianium.retirement.core
  (:require [clojure.spec.alpha :as s]
            [brianium.retirement.spec :as retirement])
  (:import (java.util Calendar)))

(defn get-year
  [date]
  (-> (doto (Calendar/getInstance) (.setTime date))
      (.get Calendar/YEAR)))

(s/fdef get-year
  :args (s/cat :date ::retirement/date)
  :ret  ::retirement/year)

(defprotocol Retiree
  (-years-until-retired [r] "How many years until the retiree retires")
  (-retire-by-year [r date]))

(defrecord User [age retirement-age]
  Retiree
  (-years-until-retired [_]
    (- retirement-age age))
  (-retire-by-year [self date]
    (-> date
        (get-year)
        (+ (-years-until-retired self)))))

(defn years-until-retired
  [retiree]
  (-years-until-retired retiree))

(s/fdef years-until-retired
  :args (s/cat :retiree ::retirement/retiree)
  :ret  int?)

(defn retire-by-year
  [retiree date]
  (-retire-by-year retiree date))

(s/fdef retire-by-year
  :args (s/cat :retiree ::retirement/retiree :date ::retirement/date)
  :ret  ::retirement/year)

(defn make-user
  [age retirement-age]
  (->User age retirement-age))

(s/fdef make-user
  :args (s/cat :age ::retirement/age :retirement-age ::retirement/retirement-age)
  :ret  ::retirement/retiree)
